import React, { useEffect, useState } from "react";
import type { UnitAmmoSlotDto } from "@/api/exvs";
import { deleteUnitAmmoSlot } from "@/api/wrapper/stats-api";
import { PlusIcon } from "@radix-ui/react-icons";
import { MdDelete, MdEdit } from "react-icons/md";
import { Link, useParams } from "@tanstack/react-router";

import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { toast } from "@/components/ui/use-toast";

import CreateAmmoSlotSheet from "./components/create-ammo-slot-sheet";
import UpdateAmmoSlotSheet from "./components/update-ammo-slot-sheet";
import { AmmoSlotContext, AmmoSlotContextType } from "./types";

const AmmoSlots = ({
  unitId,
  ammoSlots,
  ammoOptions,
}: {
  unitId: number;
  ammoSlots: UnitAmmoSlotDto[];
  ammoOptions: number[];
}) => {
  const [ammoSlotsData, setAmmoSlotsData] =
    useState<UnitAmmoSlotDto[]>(ammoSlots);
  const setAmmoSlots = (data: UnitAmmoSlotDto[]) => {
    setAmmoSlotsData([...data]);
  };
  const params = useParams();

  useEffect(() => {
    setAmmoSlotsData(ammoSlots);
  }, [ammoSlots]);

  const deleteAmmoSlot = async (index: number) => {
    const id = ammoSlotsData[index].id;

    if (id) {
      await deleteUnitAmmoSlot(id);
      const updatedAmmoSlotsData = ammoSlotsData.filter(
        (item) => item.id !== id,
      );
      setAmmoSlotsData(updatedAmmoSlotsData);

      toast({
        title: `Ammo Slot ${index + 1} Deleted!`,
      });
    }
  };

  return (
    <AmmoSlotContext.Provider
      value={{ ammoSlots: ammoSlotsData, setAmmoSlots: setAmmoSlots }}
    >
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        {Array.from({ length: 4 }, (_, i) =>
          typeof ammoSlotsData[i] === "undefined" ? (
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
                      <div className="flex items-center gap-3">
                        <UpdateAmmoSlotSheet
                          unitId={unitId}
                          index={i}
                          ammoOptions={ammoOptions}
                        >
                          <Button variant={"outline"} size="sm">
                            <MdEdit className="h-4 w-4" aria-hidden="true" />
                          </Button>
                        </UpdateAmmoSlotSheet>
                        <AlertDialog>
                          <AlertDialogTrigger asChild>
                            <Button variant={"destructive"} size="sm">
                              <MdDelete
                                className="h-4 w-4"
                                aria-hidden="true"
                              />
                            </Button>
                          </AlertDialogTrigger>
                          <AlertDialogContent>
                            <AlertDialogHeader>
                              <AlertDialogTitle>Are you sure?</AlertDialogTitle>
                            </AlertDialogHeader>
                            <AlertDialogDescription>
                              This will unassign the ammo to this slot.
                            </AlertDialogDescription>
                            <AlertDialogFooter>
                              <AlertDialogCancel>Cancel</AlertDialogCancel>
                              <AlertDialogAction
                                onClick={() => deleteAmmoSlot(i)}
                              >
                                Continue
                              </AlertDialogAction>
                            </AlertDialogFooter>
                          </AlertDialogContent>
                        </AlertDialog>
                      </div>
                    </div>
                  </CardTitle>
                </CardHeader>
                <CardContent>
                  <Link
                    to={`/units/${params.unitId}/info/ammo/?hash=${ammoSlotsData[i].ammoHash}`}
                  >
                    <div className="text-2xl font-bold">
                      {ammoSlotsData[i].ammoHash}
                    </div>
                  </Link>
                </CardContent>
              </Card>
            </div>
          ),
        )}
      </div>
    </AmmoSlotContext.Provider>
  );
};

export default AmmoSlots;
