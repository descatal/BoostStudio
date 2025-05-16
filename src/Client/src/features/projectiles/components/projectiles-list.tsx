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
        <CardHeader className={"flex flex-row justify-between"}>
          <div>
            <CardTitle className={"mb-2"}>Projectiles</CardTitle>
            <CardDescription>
              {unitId
                ? "Projectiles associated with this unit"
                : "All projectiles"}
            </CardDescription>
          </div>
          <ProjectileExportDialog unitIds={unitId ? [unitId] : undefined} />
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
