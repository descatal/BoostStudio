import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { useQuery } from "@tanstack/react-query";
import { getApiUnitStatsByUnitIdOptions } from "@/api/exvs/@tanstack/react-query.gen";
import SelectAmmoSlotDialog from "@/features/stats/components/dialogs/select-ammo-slot-dialog.tsx";

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

  // const ammoQuery = useQuery({
  //   ...getApiAmmoOptions({
  //     query: {
  //       UnitIds: [unitId],
  //     },
  //   }),
  // });

  const ammoSlots = unitStatQuery.data?.ammoSlots ?? [];
  // const ammoOptions = ammoQuery.data?.items.map((dto) => dto.hash!) ?? [];

  return (
    <Card className="col-span-full">
      <CardHeader>
        <CardTitle>
          <h2 className="text-2xl font-bold tracking-tight">Ammo Slots</h2>
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
                      <SelectAmmoSlotDialog
                        index={i}
                        unitId={unitId}
                        existingData={ammoSlots[i]}
                      />
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
                    <SelectAmmoSlotDialog
                      index={i}
                      unitId={unitId}
                      existingData={ammoSlots[i]}
                    >
                      <div className="text-2xl font-bold">
                        {ammoSlots[i]?.ammoHash}
                      </div>
                    </SelectAmmoSlotDialog>
                    {/*<Link*/}
                    {/*  to={"/units/$unitId/info/ammo"}*/}
                    {/*  params={{*/}
                    {/*    unitId: unitId.toString(),*/}
                    {/*  }}*/}
                    {/*  search={{*/}
                    {/*    hash: ammoSlots[i]?.ammoHash,*/}
                    {/*  }}*/}
                    {/*>*/}
                    {/*  <div className="text-2xl font-bold">*/}
                    {/*    {ammoSlots[i]?.ammoHash}*/}
                    {/*  </div>*/}
                    {/*</Link>*/}
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
