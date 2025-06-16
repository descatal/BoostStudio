import React from "react";
import {
  Carousel,
  CarouselApi,
  CarouselContent,
  CarouselItem,
} from "@/components/ui/carousel.tsx";
import { useQuery } from "@tanstack/react-query";
import { getApiSeriesOptions } from "@/api/exvs/@tanstack/react-query.gen.ts";
import { Card, CardContent } from "@/components/ui/card";
import { cn } from "@/lib/utils";
import { WheelGesturesPlugin } from "embla-carousel-wheel-gestures";
import { Skeleton } from "@/components/ui/skeleton";
import { useCarouselTweenEffect } from "@/hooks/use-carousel-tween-effect.tsx";

interface Props extends React.ComponentPropsWithoutRef<typeof Carousel> {
  value?: number;
  onValueChange?: (value: number) => void;
  search?: string;
}

const SeriesCarousel = ({ value, onValueChange, search, ...props }: Props) => {
  const [api, setApi] = React.useState<CarouselApi>();

  useCarouselTweenEffect(api, { tweenFactorBase: 0.6, effectType: "opacity" });

  const query = useQuery({
    ...getApiSeriesOptions({
      query: {
        Search: search ? [search] : undefined,
        ListAll: true,
      },
    }),
  });

  const data = query.data?.items ?? [];

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
      orientation={"vertical"}
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
      <CarouselContent
        className={"h-[calc(100svh-500px)] md:h-[calc(100svh-300px)]"}
      >
        {query.isFetching ? (
          <div className={"flex flex-col gap-4"}>
            <Skeleton className="h-[150px] rounded-xl" />
            <Skeleton className="h-[150px] rounded-xl" />
            <Skeleton className="h-[150px] rounded-xl" />
          </div>
        ) : (
          data.map((item, index) => (
            <CarouselItem key={index} className="content-center basis-1/8">
              <div className="p-1">
                <Card className={cn("transition-transform duration-500")}>
                  <CardContent className="h-20 p-0">
                    <img
                      className={
                        "object-scale-down text-xs text-center align-middle"
                      }
                      alt={item.nameEnglish!}
                    />
                  </CardContent>
                  {/*<CardFooter className={"w-full justify-center"}>*/}
                  {/*  <span className="text-xs">{item.nameEnglish}</span>*/}
                  {/*</CardFooter>*/}
                </Card>
              </div>
            </CarouselItem>
          ))
        )}
      </CarouselContent>
      {/*{query.isSuccess && (*/}
      {/*  <>*/}
      {/*    <div className="absolute left-1/2 top-2 flex items-center justify-center">*/}
      {/*      <CarouselPrevious className="relative top-0 translate-y-0 hover:translate-y-0" />*/}
      {/*    </div>*/}
      {/*    <div className="absolute left-1/2 bottom-2 flex items-center justify-center">*/}
      {/*      <CarouselNext className="relative bottom-0 translate-y-0 hover:translate-y-0" />*/}
      {/*    </div>*/}
      {/*  </>*/}
      {/*)}*/}
    </Carousel>
  );
};

export default SeriesCarousel;
