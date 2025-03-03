import React from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import ProjectilesTable from "@/features/projectiles/components/projectiles-table/projectiles-table";

interface ProjectilesListProps {
  unitId?: number | undefined;
}

const ProjectilesList = ({ unitId }: ProjectilesListProps) => {
  return (
    <div className={"flex flex-col gap-3"}>
      <Card className="col-span-full">
        <CardHeader>
          <CardTitle>Projectiles</CardTitle>
          <CardDescription>
            {unitId
              ? "Projectiles associated with this unit"
              : "All projectiles"}
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <ProjectilesTable unitId={unitId} />
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default ProjectilesList;
