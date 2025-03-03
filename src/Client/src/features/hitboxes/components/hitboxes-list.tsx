import React from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import HitboxesTable from "@/features/hitboxes/components/hitboxes-table/hitboxes-table";

interface HitboxesListProps {
  unitId?: number | undefined;
}

const HitboxesList = ({ unitId }: HitboxesListProps) => {
  return (
    <div className={"flex flex-col gap-3"}>
      <Card className="col-span-full">
        <CardHeader>
          <CardTitle>Hitboxes</CardTitle>
          <CardDescription>
            {unitId ? "Hitboxes associated with this unit" : "All hitboxes"}
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <HitboxesTable unitId={unitId} />
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default HitboxesList;
