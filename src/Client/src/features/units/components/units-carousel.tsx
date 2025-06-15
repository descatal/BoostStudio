import React from "react";
import {
  Carousel,
  CarouselApi,
  CarouselContent,
  CarouselItem,
} from "@/components/ui/carousel.tsx";
import { useQuery } from "@tanstack/react-query";
import { getApiSeriesUnitsOptions } from "@/api/exvs/@tanstack/react-query.gen.ts";
import { Card, CardContent } from "@/components/ui/card";
import { cn } from "@/lib/utils";
import { WheelGesturesPlugin } from "embla-carousel-wheel-gestures";
import { Skeleton } from "@/components/ui/skeleton.tsx";
import { useCarouselTweenEffect } from "@/hooks/useCarouselTweenEffect.tsx";

interface Props extends React.ComponentPropsWithoutRef<typeof Carousel> {
  value?: number;
  onValueChange?: (value: number) => void;
  search?: string;
}

const UnitsCarousel = ({ value, onValueChange, search, ...props }: Props) => {
  const [api, setApi] = React.useState<CarouselApi>();

  useCarouselTweenEffect(api, { tweenFactorBase: 0.2, effectType: "scale" });

  const query = useQuery({
    ...getApiSeriesUnitsOptions({
      query: {
        UnitIds: value ? [value] : undefined,
      },
    }),
  });

  const data = query.data?.items.flatMap((vm) => vm.units ?? []) ?? [];

  React.useEffect(() => {
    if (!api || !onValueChange) {
      return;
    }

    onValueChange(api.selectedScrollSnap() + 1);
    api.on("select", () => {
      onValueChange(api.selectedScrollSnap() + 1);
    });
  }, [api]);

  return (
    <Carousel
      setApi={setApi}
      plugins={[
        WheelGesturesPlugin({
          forceWheelAxis: "y",
        }),
      ]}
      opts={{
        align: "center",
        loop: true,
        skipSnaps: true,
      }}
      {...props}
    >
      <CarouselContent>
        {query.isFetching ? (
          <div className={"w-full flex gap-4"}>
            <Skeleton className="h-[150px] rounded-xl" />
            <Skeleton className="h-[125px] rounded-xl" />
            <Skeleton className="h-[125px] rounded-xl" />
          </div>
        ) : (
          data.map((item, index) => (
            <CarouselItem
              key={index}
              className="content-center basis-1/2 md:basis-1/4"
            >
              <div className="p-1">
                <Card className={cn("transition-transform duration-500")}>
                  <CardContent className="h-[calc(100svh-400px)] p-0">
                    <img
                      className={
                        "object-cover text-xs text-center align-middle"
                      }
                      alt={item.nameEnglish!}
                    />
                  </CardContent>
                  {/*<CardFooter className={"w-full justify-center"}>*/}
                  {/*  <span className="text-xs">{item.na308830meEnglish}</span>*/}
                  {/*</CardFooter>*/}
                </Card>
              </div>
            </CarouselItem>
          ))
        )}
      </CarouselContent>
      {/*{query.isSuccess && (*/}
      {/*  <>*/}
      {/*    <div className="absolute top-1/2 left-2 flex items-center justify-center">*/}
      {/*      <CarouselPrevious className="relative left-0 translate-x-0 hover:translate-x-0" />*/}
      {/*    </div>*/}
      {/*    <div className="absolute top-1/2 right-2 flex items-center justify-center">*/}
      {/*      <CarouselNext className="relative right-0 translate-x-0 hover:translate-x-0" />*/}
      {/*    </div>*/}
      {/*  </>*/}
      {/*)}*/}
    </Carousel>
  );
};

export default UnitsCarousel;
