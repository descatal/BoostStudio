import React from "react";
import { Separator } from "@/components/ui/separator";
import { toast } from "@/hooks/use-toast";
import { Icons } from "@/components/icons";
import { Switch } from "@/components/ui/switch";
import { useMutation } from "@tanstack/react-query";
import { postApiAmmoExportMutation } from "@/api/exvs/@tanstack/react-query.gen";
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

interface AmmoExportDialogProps
  extends Omit<React.ComponentPropsWithRef<typeof Credenza>, "children"> {
  triggerButton?: React.ReactNode;
}

const AmmoExportDialog = ({
  triggerButton,
  ...props
}: AmmoExportDialogProps) => {
  const [hotReload, setHotReload] = React.useState(true);

  const mutation = useMutation({
    ...postApiAmmoExportMutation(),
    onSuccess: (_) => {
      toast({
        title: "Success",
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
            Export ammo info for ALL units to working directory.
          </CredenzaDescription>
        </CredenzaHeader>
        <CredenzaBody>
          <div className="grid gap-4">
            <div className="flex items-center space-x-4 rounded-md border p-4">
              <MdMemory size={40} />
              <div className="flex-1 space-y-1">
                <p className="text-sm font-medium leading-none">Hot Reload</p>
                <p className="text-sm text-muted-foreground">
                  Patch the compiled binary to a running RPCS3's memory.
                </p>
              </div>
              <Switch checked={hotReload} onCheckedChange={setHotReload} />
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

export default AmmoExportDialog;
