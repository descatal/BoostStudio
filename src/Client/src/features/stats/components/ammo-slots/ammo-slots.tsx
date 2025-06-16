import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { PlusIcon } from "@radix-ui/react-icons";
import { Link } from "@tanstack/react-router";
import { useQuery } from "@tanstack/react-query";
import {
  getApiAmmoOptions,
  getApiUnitStatsByUnitIdOptions,
} from "@/api/exvs/@tanstack/react-query.gen";
import CreateAmmoSlotSheet from "@/components/create-ammo-slot-sheet";

interface AmmoSlotsProps {
  unitId: number;
}

const AmmoSlots = ({ unitId }: AmmoSlotsProps) => {
  const unitStatQuery = useQuery({
    ...getApiUnitStatsByUnitIdOptions({
      path: {
        unitId: unitId,
      },
    }),
  });

  const ammoQuery = useQuery({
    ...getApiAmmoOptions({
      query: {
        UnitIds: [unitId],
      },
    }),
  });

  const ammoSlots = unitStatQuery.data?.ammoSlots ?? [];
  const ammoOptions = ammoQuery.data?.items.map((dto) => dto.hash!) ?? [];

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
