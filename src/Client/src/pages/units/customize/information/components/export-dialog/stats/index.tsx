import React from "react";
import { UnitSummaryVm } from "@/api/exvs";
import { exportUnitStats } from "@/api/wrapper/stats-api";
import UnitsSelector from "@/features/units/components/units-selector";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Label } from "@/components/ui/label";
import { Separator } from "@/components/ui/separator";
import { toast } from "@/components/ui/use-toast";
import { Icons } from "@/components/icons";

interface StatsExportProps {
  units?: UnitSummaryVm[] | undefined;
  onExport: () => void;
}

const StatsExport = ({ units, onExport }: StatsExportProps) => {
  const [isExportPending, setIsExportPending] = React.useState(false);
  const [hotReload, setHotReload] = React.useState(true);

  const [selectedExportUnits, setSelectedExportUnits] =
    React.useState<UnitSummaryVm[]>();

  React.useEffect(() => {
    if (units) {
      setSelectedExportUnits(units);
    }
  }, [units]);

  const handleExport = async () => {
    try {
      await exportUnitStats({
        exportUnitStatCommand: {
          unitIds: selectedExportUnits?.map((x) => x.unitId!),
          replaceWorking: true,
        },
      });

      toast({
        title: "Success",
        description: `Successfully exported unit stats binary to working directory!`,
      });

      onExport();
    } catch (e) {
      toast({
        title: `Error`,
        description: `Export failed! ${e}`,
        variant: "destructive",
      });
    }
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Export Unit Stats Info</CardTitle>
        <CardDescription>
          Export unit stats info for this unit to working directory.
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="grid gap-4">
          <div className={"space-y-2"}>
            <Label>Units</Label>
            <UnitsSelector
              disabled={!!units}
              multipleSelect={true}
              selectedUnits={selectedExportUnits}
              setSelectedUnits={setSelectedExportUnits}
            />
          </div>
          {/*<div className="flex items-center space-x-4 rounded-md border p-4">*/}
          {/*  <ArrowBigDownDash/>*/}
          {/*  <div className="flex-1 space-y-1">*/}
          {/*    <p className="text-sm font-medium leading-none">Hot Reload</p>*/}
          {/*    <p className="text-sm text-muted-foreground">*/}
          {/*      Patch the compiled unit stats binary to running RPCS3.*/}
          {/*    </p>*/}
          {/*  </div>*/}
          {/*  <Switch checked={hotReload} onCheckedChange={setHotReload}/>*/}
          {/*</div>*/}
          <Separator />
          <Button
            disabled={isExportPending}
            onClick={async () => {
              setIsExportPending(true);
              await handleExport();
              setIsExportPending(false);
            }}
          >
            {isExportPending && (
              <Icons.spinner
                className="mr-2 size-4 animate-spin"
                aria-hidden="true"
              />
            )}
            Export
          </Button>
        </div>
      </CardContent>
    </Card>
  );
};

export default StatsExport;
