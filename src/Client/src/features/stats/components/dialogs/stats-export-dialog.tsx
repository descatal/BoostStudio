import React from "react";
import { Icons } from "@/components/icons";
import { useMutation } from "@tanstack/react-query";
import { postApiUnitStatsExportMutation } from "@/api/exvs/@tanstack/react-query.gen";
import {
  Credenza,
  CredenzaContent,
  CredenzaDescription,
  CredenzaFooter,
  CredenzaHeader,
  CredenzaTitle,
  CredenzaTrigger,
} from "@/components/credenza";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { BiExport } from "react-icons/bi";
import { toast } from "sonner";

interface StatsExportDialogProps
  extends Omit<React.ComponentPropsWithRef<typeof Credenza>, "children"> {
  unitIds?: number[] | undefined;
  children?: React.ReactNode;
}

const StatsExportDialog = ({
  unitIds,
  children,
  ...props
}: StatsExportDialogProps) => {
  const [open, setOpen] = React.useState(false);
  const mutation = useMutation({
    ...postApiUnitStatsExportMutation(),
    onSuccess: (_) => {
      toast("Success", {
        description: `Export completed!`,
      });
      setOpen(false);
    },
  });

  return (
    <Credenza {...props} open={open} onOpenChange={setOpen}>
      <CredenzaTrigger asChild>
        {children ?? (
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
          <CredenzaTitle>Export Stats Info</CredenzaTitle>
          <CredenzaDescription>
            Export stats info for units to working directory.
          </CredenzaDescription>
        </CredenzaHeader>
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
                  unitIds: unitIds,
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

export default StatsExportDialog;
