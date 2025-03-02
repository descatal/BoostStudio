import { createFileRoute, Link } from "@tanstack/react-router";
import { Header } from "@/components/layout/header";
import { Search } from "@/components/search";
import { ThemeSwitch } from "@/components/theme-switch";
import { Main } from "@/components/layout/main";
import { UnitSummaryVm } from "@/api/exvs";
import React, { useState } from "react";
import { useSeriesUnits } from "@/features/series/api/get-series";
import UnitCard from "@/pages/units/components/unit-card";
import { Separator } from "@/components/ui/separator";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

export const Route = createFileRoute("/units/")({
  component: RouteComponent,
});

function RouteComponent() {
  const [selectedUnit, setSelectedUnit] = useState<UnitSummaryVm | undefined>(
    undefined,
  );
  const [search, setSearch] = useState("");

  const seriesUnitsQuery = useSeriesUnits();
  const seriesUnits = seriesUnitsQuery.data?.items;

  return (
    <>
      {/* ===== Top Heading ===== */}
      <Header>
        <div className="ml-auto flex items-center space-x-4">
          <Search />
          <ThemeSwitch />
        </div>
      </Header>

      <Main fixed>
        <div className={"flex flex-row w-full justify-between"}>
          <Input
            placeholder={"Search units"}
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className={"m-2 h-8 w-[300px]"}
          />
          {selectedUnit && (
            <Link
              to="/units/$unitId"
              params={{ unitId: selectedUnit.unitId!.toString() }}
            >
              <Button>Edit</Button>
            </Link>
          )}
        </div>

        {seriesUnits &&
          seriesUnits.map((vm) => (
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
                      onClick={() => setSelectedUnit(unit)}
                      key={unit.unitId}
                      unit={unit}
                      selected={selectedUnit?.unitId === unit.unitId}
                    />
                  ))}
              </div>
              <Separator />
            </div>
          ))}
      </Main>
    </>
  );
}
