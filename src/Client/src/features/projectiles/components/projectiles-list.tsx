import React from "react";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import ProjectilesTable from "@/features/projectiles/components/table/table";
import ProjectileExportDialog from "@/features/projectiles/components/dialogs/export";

interface ProjectilesListProps {
  unitId?: number | undefined;
}

const ProjectilesList = ({ unitId }: ProjectilesListProps) => {
  return (
    <div className={"flex flex-col gap-3"}>
      <Card className="col-span-full">
        <div className={"flex flex-row justify-between"}>
          <CardHeader>
            <CardTitle>Projectiles</CardTitle>
            <CardDescription>
              {unitId
                ? "Projectiles associated with this unit"
                : "All projectiles"}
            </CardDescription>
          </CardHeader>
          <div className="flex items-center space-x-2 mr-5">
            <ProjectileExportDialog unitIds={unitId ? [unitId] : undefined} />
          </div>
        </div>
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
