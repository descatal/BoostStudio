import { useCallback, useEffect, useRef } from "react";
import { EmblaCarouselType, EmblaEventType } from "embla-carousel";

const numberWithinRange = (number: number, min: number, max: number): number =>
  Math.min(Math.max(number, min), max);

type TweenEffect = "scale" | "opacity" | "custom";

interface UseCarouselTweenEffectOptions {
  /** Base factor for calculating the tween effect. Higher values make the effect more dramatic. */
  tweenFactorBase?: number;
  /** CSS selector for the elements within slides that should have the effect applied */
  elementSelector?: string | null;
  /** Type of effect to apply */
  effectType?: TweenEffect;
  /** Custom effect function to apply if effectType is "custom" */
  customEffectFn?: (element: HTMLElement, tweenValue: number) => void;
}

/**
 * A custom hook that adds tween effects to carousel items based on their position.
 *
 * @param emblaApi The embla carousel API
 * @param options Configuration options
 * @returns An object containing the tween factor and nodes refs
 */
export function useCarouselTweenEffect(
  emblaApi: EmblaCarouselType | undefined,
  options: UseCarouselTweenEffectOptions = {},
) {
  const {
    tweenFactorBase = 0.6,
    elementSelector = null,
    effectType = "opacity",
    customEffectFn,
  } = options;

  const tweenFactor = useRef(0);
  const tweenNodes = useRef<HTMLElement[]>([]);

  const setTweenNodes = useCallback(
    (emblaApi: EmblaCarouselType): void => {
      if (elementSelector) {
        // Apply effect to specific elements within slides
        tweenNodes.current = emblaApi.slideNodes().map((slideNode) => {
          return slideNode.querySelector(elementSelector) as HTMLElement;
        });
      } else {
        // Apply effect to entire slides
        tweenNodes.current = emblaApi.slideNodes() as HTMLElement[];
      }
    },
    [elementSelector],
  );

  const setTweenFactor = useCallback(
    (emblaApi: EmblaCarouselType) => {
      tweenFactor.current = tweenFactorBase * emblaApi.scrollSnapList().length;
    },
    [tweenFactorBase],
  );

  const applyEffect = useCallback(
    (element: HTMLElement, tweenValue: number) => {
      if (!element) return;

      const normalizedValue = numberWithinRange(tweenValue, 0, 1).toString();

      switch (effectType) {
        case "scale":
          element.style.transform = `scale(${normalizedValue})`;
          break;
        case "opacity":
          element.style.opacity = normalizedValue;
          break;
        case "custom":
          if (customEffectFn) {
            customEffectFn(element, tweenValue);
          }
          break;
      }
    },
    [effectType, customEffectFn],
  );

  const tweenEffect = useCallback(
    (emblaApi: EmblaCarouselType, eventName?: EmblaEventType) => {
      const engine = emblaApi.internalEngine();
      const scrollProgress = emblaApi.scrollProgress();
      const slidesInView = emblaApi.slidesInView();
      const isScrollEvent = eventName === "scroll";

      emblaApi.scrollSnapList().forEach((scrollSnap, snapIndex) => {
        let diffToTarget = scrollSnap - scrollProgress;
        const slidesInSnap = engine.slideRegistry[snapIndex];

        slidesInSnap.forEach((slideIndex) => {
          if (isScrollEvent && !slidesInView.includes(slideIndex)) return;

          if (engine.options.loop) {
            engine.slideLooper.loopPoints.forEach((loopItem) => {
              const target = loopItem.target();

              if (slideIndex === loopItem.index && target !== 0) {
                const sign = Math.sign(target);

                if (sign === -1) {
                  diffToTarget = scrollSnap - (1 + scrollProgress);
                }
                if (sign === 1) {
                  diffToTarget = scrollSnap + (1 - scrollProgress);
                }
              }
            });
          }

          const tweenValue = 1 - Math.abs(diffToTarget * tweenFactor.current);
          const node = tweenNodes.current[slideIndex];

          if (node) {
            applyEffect(node, tweenValue);
          }
        });
      });
    },
    [applyEffect],
  );

  useEffect(() => {
    if (!emblaApi) return;

    setTweenNodes(emblaApi);
    setTweenFactor(emblaApi);
    tweenEffect(emblaApi);

    emblaApi
      .on("reInit", setTweenNodes)
      .on("reInit", setTweenFactor)
      .on("reInit", tweenEffect)
      .on("scroll", tweenEffect)
      .on("slideFocus", tweenEffect);

    return () => {
      // Cleanup event listeners when the component unmounts
      emblaApi
        .off("reInit", setTweenNodes)
        .off("reInit", setTweenFactor)
        .off("reInit", tweenEffect)
        .off("scroll", tweenEffect)
        .off("slideFocus", tweenEffect);
    };
  }, [emblaApi, setTweenNodes, setTweenFactor, tweenEffect]);

  return {
    tweenFactor,
    tweenNodes,
  };
}
