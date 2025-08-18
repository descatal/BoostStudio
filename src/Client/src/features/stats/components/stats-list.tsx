import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import AmmoSlots from "@/features/stats/components/ammo-slots/ammo-slots";
import StatsGroupTable from "@/features/stats/components/stats-group-table/table";
import StatsExportDialog from "@/features/stats/components/dialogs/stats-export-dialog.tsx";

interface StatsListProps {
  unitId?: number | undefined;
}

const StatsList = ({ unitId }: StatsListProps) => {
  return (
    <div className={"flex flex-col gap-3"}>
      {unitId && (
        <div className={"flex flex-col gap-3"}>
          <div className={"flex justify-end"}>
            <StatsExportDialog unitIds={[unitId]} />
          </div>
          <AmmoSlots unitId={unitId} />
        </div>
      )}
      <Card className="col-span-full">
        <CardHeader>
          <CardTitle>
            <h2 className="text-2xl font-bold tracking-tight">Stat Groups</h2>
          </CardTitle>
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
