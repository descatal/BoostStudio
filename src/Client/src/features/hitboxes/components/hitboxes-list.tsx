import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import HitboxesTable from "@/features/hitboxes/components/table/table";
import HitboxExportDialog from "@/features/hitboxes/components/dialogs/export";

interface HitboxesListProps {
  unitId?: number | undefined;
}

const HitboxesList = ({ unitId }: HitboxesListProps) => {
  return (
    <div className={"flex flex-col gap-3"}>
      <Card className="col-span-full">
        <CardHeader className={"flex flex-row justify-between"}>
          <div>
            <CardTitle>Hitboxes</CardTitle>
            <CardDescription>
              {unitId ? "Hitboxes associated with this unit" : "All hitboxes"}
            </CardDescription>
          </div>
          <HitboxExportDialog unitIds={unitId ? [unitId] : undefined} />
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
