import React from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import AmmoSlots from "@/features/stats/components/ammo-slots/ammo-slots";
import StatsGroupTable from "@/features/stats/components/stats-group-table/stats-group-table";

interface StatsListProps {
  unitId?: number | undefined;
}

const StatsList = ({ unitId }: StatsListProps) => {
  return (
    <div className={"flex flex-col gap-3"}>
      {unitId && <AmmoSlots unitId={unitId} />}
      <Card className="col-span-full">
        <CardHeader>
          <CardTitle>Stat Groups</CardTitle>
          <CardDescription>
            Stat groups associated with this unit.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <StatsGroupTable unitId={unitId} />
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default StatsList;
