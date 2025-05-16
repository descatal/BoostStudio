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
        <CardHeader className={"flex flex-row justify-between"}>
          <div>
            <CardTitle>Ammo</CardTitle>
            <CardDescription>
              {unitId ? "Ammo associated with this unit" : "All ammo"}
            </CardDescription>
          </div>
          <AmmoExportDialog />
        </CardHeader>
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
