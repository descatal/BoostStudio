import { createFileRoute } from "@tanstack/react-router";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import UnitsCarousel from "@/features/units/components/units-carousel.tsx";
import UnitsSelector from "@/features/units/components/units-selector.tsx";
import SeriesCarousel from "@/features/series/components/series-carousel.tsx";
import { Separator } from "@/components/ui/separator.tsx";
import React from "react";

export const Route = createFileRoute("/units/")({
  component: RouteComponent,
});

function RouteComponent() {
  // const seriesUnitsQuery = useQuery({
  //   ...getApiSeriesUnitsOptions({
  //     query: {
  //       ListAll: true,
  //     },
  //   }),
  //   select: (data) =>
  //     data?.items
  //       .filter((vm) => vm.units)
  //       .flatMap((vm) => vm.units!)
  //       .map((type) => type.unitId!) ?? [],
  // });
  //
  // const seriesUnits = seriesUnitsQuery.data ?? [];

  const [selectedSeries, setSelectedSeries] = React.useState(0);
  // const [selectedUnit, setSelectedUnit] = React.useState();

  return (
    <div className={"flex justify-center w-full"}>
      <Card className={"w-full"}>
        <div className={"flex justify-between w-full items-center"}>
          <CardHeader className={"w-full"}>
            <CardTitle className={"text-2xl"}>Units</CardTitle>
            <CardDescription className={"text-sm"}>
              Select a unit to customize.
            </CardDescription>
          </CardHeader>
          <div className={"mr-6 w-64 md:w-[800px]"}>
            <UnitsSelector placeholder={"Search unit..."} />
          </div>
        </div>
        <CardContent className={"w-full flex"}>
          <div className={"grid grid-cols-12"}>
            <SeriesCarousel
              className={
                "flex justify-self-start items-center col-span-2 w-full"
              }
              value={selectedSeries}
              onValueChange={setSelectedSeries}
            />
            <div className={"col-span-9 flex"}>
              <Separator orientation={"vertical"} className={"mx-4"} />
              <UnitsCarousel className={"content-center w-full"} />
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
