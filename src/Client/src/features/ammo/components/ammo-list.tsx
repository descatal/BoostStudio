import React from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import AmmoTable from "@/features/ammo/components/table/table";
import AmmoExportDialog from "@/features/ammo/components/dialogs/export";

interface AmmoListProps {
  unitId?: number | undefined;
}

const AmmoList = ({ unitId }: AmmoListProps) => {
  return (
    <div className={"flex flex-col gap-3"}>
      <Card className="col-span-full">
        <div className={"flex flex-row justify-between"}>
          <CardHeader>
            <CardTitle>Ammo</CardTitle>
            <CardDescription>
              {unitId ? "Ammo associated with this unit" : "All ammo"}
            </CardDescription>
          </CardHeader>
          <div className="flex items-center space-x-2 mr-5">
            <AmmoExportDialog />
          </div>
        </div>
        <CardContent>
          <div className="space-y-4">
            <AmmoTable unitId={unitId} />
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default AmmoList;
