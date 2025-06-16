import { useCallback, useEffect, useRef } from "react";
import { CarouselApi } from "@/components/ui/carousel";
import { EmblaCarouselType, EmblaEventType } from "embla-carousel";

const numberWithinRange = (number: number, min: number, max: number): number =>
  Math.min(Math.max(number, min), max);

interface UseCarouselTweenOpacityOptions {
  /** Base factor for calculating the tween effect. Higher values make the opacity change more dramatic. */
  tweenFactorBase?: number;
}

/**
 * A custom hook that adds a tween opacity effect to carousel items based on their position.
 *
 * @param api The carousel API
 * @param options Configuration options
 * @returns An object containing the tween factor ref
 */
export function useCarouselTweenOpacity(
  api: CarouselApi | undefined,
  options: UseCarouselTweenOpacityOptions = {},
) {
  const { tweenFactorBase = 0.6 } = options;
  const tweenFactor = useRef(0);

  const setTweenFactor = useCallback(
    (emblaApi: EmblaCarouselType) => {
      tweenFactor.current = tweenFactorBase * emblaApi.scrollSnapList().length;
    },
    [tweenFactorBase],
  );

  const tweenOpacity = useCallback(
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
          emblaApi.slideNodes()[slideIndex].style.opacity = numberWithinRange(
            tweenValue,
            0,
            1,
          ).toString();
        });
      });
    },
    [tweenFactor],
  );

  useEffect(() => {
    if (!api) return;

    setTweenFactor(api);
    tweenOpacity(api);

    api
      .on("reInit", setTweenFactor)
      .on("reInit", tweenOpacity)
      .on("scroll", tweenOpacity)
      .on("slideFocus", tweenOpacity);

    return () => {
      // Cleanup event listeners when the component unmounts
      api
        .off("reInit", setTweenFactor)
        .off("reInit", tweenOpacity)
        .off("scroll", tweenOpacity)
        .off("slideFocus", tweenOpacity);
    };
  }, [api, setTweenFactor, tweenOpacity]);

  return {
    tweenFactor,
  };
}
