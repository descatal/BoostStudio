import React from "react";
import { Separator } from "@/components/ui/separator";
import { Icons } from "@/components/icons";
import { Switch } from "@/components/ui/switch";
import { useMutation } from "@tanstack/react-query";
import { postApiHitboxGroupsExportMutation } from "@/api/exvs/@tanstack/react-query.gen";
import {
  Credenza,
  CredenzaBody,
  CredenzaContent,
  CredenzaDescription,
  CredenzaFooter,
  CredenzaHeader,
  CredenzaTitle,
  CredenzaTrigger,
} from "@/components/credenza";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { BiExport } from "react-icons/bi";
import { MdMemory } from "react-icons/md";
import { Label } from "@/components/ui/label";
import UnitsSelector from "@/features/units/components/units-selector";
import { Option } from "@/components/ui/multiple-selector";
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from "@/components/ui/tooltip";
import { toast } from "sonner";

interface HitboxExportDialogProps
  extends Omit<React.ComponentPropsWithRef<typeof Credenza>, "children"> {
  triggerButton?: React.ReactNode;
  unitIds?: number[];
}

const HitboxExportDialog = ({
  triggerButton,
  unitIds,
  ...props
}: HitboxExportDialogProps) => {
  const [hotReload, setHotReload] = React.useState(true);
  const [selectedUnitIds, setSelectedUnitIds] = React.useState<Option[]>([]);

  const mutation = useMutation({
    ...postApiHitboxGroupsExportMutation(),
    onSuccess: (_) => {
      toast("Success", {
        description: `Export completed!`,
      });
    },
  });

  return (
    <Credenza {...props}>
      <CredenzaTrigger asChild>
        {triggerButton ?? (
          <EnhancedButton
            effect={"gooeyRight"}
            icon={BiExport}
            iconPlacement={"right"}
          >
            Export
          </EnhancedButton>
        )}
      </CredenzaTrigger>
      <CredenzaContent>
        <CredenzaHeader>
          <CredenzaTitle>Export Ammo Info</CredenzaTitle>
          <CredenzaDescription>
            Export hitbox info to working directory.
          </CredenzaDescription>
        </CredenzaHeader>
        <CredenzaBody>
          <div className="grid gap-4">
            <div className={"space-y-2"}>
              <Label>Units</Label>
              <UnitsSelector
                disabled={!!unitIds}
                className={"w-full"}
                defaultValues={unitIds}
                value={selectedUnitIds}
                onChange={setSelectedUnitIds}
                placeholder={unitIds ? undefined : "Select units..."}
              />
            </div>
            <div className="flex items-center space-x-4 rounded-md border p-4">
              <MdMemory size={40} />
              <div className="flex-1 space-y-1">
                <p className="text-sm font-medium leading-none">Hot Reload</p>
                <p className="text-sm text-muted-foreground">
                  Patch the compiled binary to a running RPCS3's memory.
                </p>
              </div>
              <Tooltip>
                <TooltipTrigger>
                  <Switch
                    disabled={!!unitIds && unitIds.length > 1}
                    checked={hotReload}
                    onCheckedChange={setHotReload}
                  />
                </TooltipTrigger>
                {!!unitIds && unitIds.length > 1 && (
                  <TooltipContent>
                    <p>Only available for single unit</p>
                  </TooltipContent>
                )}
              </Tooltip>
            </div>
            <Separator />
          </div>
        </CredenzaBody>
        <CredenzaFooter>
          <EnhancedButton
            className={"w-full"}
            effect={"expandIcon"}
            variant={"default"}
            icon={BiExport}
            iconPlacement={"right"}
            disabled={mutation.isPending}
            onClick={async () => {
              mutation.mutate({
                body: {
                  unitIds: selectedUnitIds.map((x) => Number(x.value)),
                  replaceWorking: true,
                  hotReload: hotReload,
                },
              });
            }}
          >
            {mutation.isPending && (
              <Icons.spinner
                className="size-4 mr-2 animate-spin"
                aria-hidden="true"
              />
            )}
            Export
          </EnhancedButton>
        </CredenzaFooter>
      </CredenzaContent>
    </Credenza>
  );
};

export default HitboxExportDialog;
