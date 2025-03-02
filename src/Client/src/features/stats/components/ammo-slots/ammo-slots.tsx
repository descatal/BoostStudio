import React from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { useApiAmmoOptions } from "@/features/ammo/api/get-ammo-options";
import { useApiUnitStatsByUnitId } from "@/features/stats/api/get-unit-stats-by-unit-id";
import CreateAmmoSlotSheet from "@/pages/units/customize/information/components/tabs/stats/components/ammo-slots/components/create-ammo-slot-sheet";
import { Button } from "@/components/ui/button";
import { PlusIcon } from "@radix-ui/react-icons";
import { Link } from "@tanstack/react-router";

interface AmmoSlotsProps {
  unitId: number;
}

const AmmoSlots = ({ unitId }: AmmoSlotsProps) => {
  const getUnitStatsByUnitIdQuery = useApiUnitStatsByUnitId({
    unitId: unitId,
  });
  const getAmmoOptionsQuery = useApiAmmoOptions({
    unitIds: [unitId],
  });

  const ammoSlots = getUnitStatsByUnitIdQuery.data?.ammoSlots ?? [];
  const ammoOptions = getAmmoOptionsQuery.data ?? [];

  return (
    <Card className="col-span-full">
      <CardHeader>
        <CardTitle>
          <div className="flex items-center justify-between space-y-2">
            <h2 className="text-2xl font-bold tracking-tight">Ammo Slots</h2>
          </div>
        </CardTitle>
        <CardDescription>
          The ammo assigned to each ammo slot when spawning this unit.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
          {Array.from({ length: 4 }, (_, i) =>
            typeof ammoSlots[i] === "undefined" ? (
              <div key={i}>
                <div className={"h-full w-full"}>
                  <Card className={"flex h-full items-center justify-center"}>
                    <CardContent>
                      <CreateAmmoSlotSheet
                        index={i}
                        unitId={unitId}
                        ammoOptions={ammoOptions}
                      >
                        <Button className={"mt-5"} variant={"ghost"}>
                          <PlusIcon className={"h-6 w-6"} />
                        </Button>
                      </CreateAmmoSlotSheet>
                    </CardContent>
                  </Card>
                </div>
              </div>
            ) : (
              <div key={i}>
                <Card>
                  <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                    <CardTitle className="w-full text-sm font-medium">
                      <div className="flex items-center justify-between">
                        <h2>Ammo Slot {i + 1}</h2>
                        <div className="flex items-center gap-3"></div>
                      </div>
                    </CardTitle>
                  </CardHeader>
                  <CardContent>
                    <Link
                      to={"/units/$unitId/info/ammo"}
                      params={{
                        unitId: unitId.toString(),
                      }}
                      search={{
                        hash: ammoSlots[i]?.ammoHash,
                      }}
                    >
                      <div className="text-2xl font-bold">
                        {ammoSlots[i]?.ammoHash}
                      </div>
                    </Link>
                  </CardContent>
                </Card>
              </div>
            ),
          )}
        </div>
      </CardContent>
    </Card>
  );
};

export default AmmoSlots;
