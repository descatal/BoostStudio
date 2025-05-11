import { createFileRoute, Link } from "@tanstack/react-router";
import { Main } from "@/components/layout/main";
import { UnitSummaryVm } from "@/api/exvs";
import React, { useState } from "react";
import UnitCard from "@/pages/units/components/unit-card";
import { Separator } from "@/components/ui/separator";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Card, CardContent } from "@/components/ui/card";
import { useQuery } from "@tanstack/react-query";
import { getApiSeriesUnitsOptions } from "@/api/exvs/@tanstack/react-query.gen";

export const Route = createFileRoute("/units/")({
  component: RouteComponent,
});

function RouteComponent() {
  const [selectedUnit, setSelectedUnit] = useState<UnitSummaryVm | undefined>(
    undefined,
  );
  const [search, setSearch] = useState("");

  const seriesUnitsQuery = useQuery({
    ...getApiSeriesUnitsOptions({
      query: {
        ListAll: true,
      },
    }),
  });

  const seriesUnits = seriesUnitsQuery.data?.items;
  const filteredUnits = seriesUnits
    ?.map((x) => {
      return {
        ...x,
        units:
          x.units?.filter(
            (unit) =>
              unit.nameEnglish!.toLowerCase().indexOf(search.toLowerCase()) >
              -1,
          ) ?? [],
      };
    })
    .filter((x) => x.units!.length > 0);

  return (
    <Main fixed>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-3xl font-bold tracking-tight">Units</h2>
        <div className="flex items-center space-x-2">
          <Input
            placeholder={"Search units"}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className={"m-2 h-8 w-[150px] sm:w-[300px]"}
          />
          <Link
            to="/units/$unitId"
            params={{ unitId: selectedUnit?.unitId?.toString() ?? "" }}
          >
            <Button disabled={!selectedUnit}>Details</Button>
          </Link>
        </div>
      </div>
      <label className="text-sm text-muted-foreground">Select Unit</label>
      <Card>
        <CardContent>
          {filteredUnits &&
            filteredUnits.map((vm) => (
              <div key={vm.id} className={"flex flex-col items-center p-4"}>
                {vm.nameEnglish ?? "Unknown"}
                <div
                  className={
                    "grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 p-6"
                  }
                >
                  {vm.units &&
                    vm.units.map((unit) => (
                      <UnitCard
                        className={"cursor-pointer"}
                        onClick={() => {
                          if (selectedUnit == unit) setSelectedUnit(undefined);
                          else setSelectedUnit(unit);
                        }}
                        key={unit.unitId}
                        unit={unit}
                        selected={selectedUnit?.unitId === unit.unitId}
                      />
                    ))}
                </div>
                <Separator />
              </div>
            ))}
        </CardContent>
      </Card>
    </Main>
  );
}
