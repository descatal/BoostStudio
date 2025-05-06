import React from "react";
import ConfirmationDialog from "@/components/custom/confirmation-dialog";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import {
  deleteApiPatchFilesByIdMutation,
  getApiPatchFilesSummaryQueryKey,
} from "@/api/exvs/@tanstack/react-query.gen";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { LuTrash2 } from "react-icons/lu";
import { Icons } from "@/components/icons";
import { toast } from "@/hooks/use-toast";

interface DeletePatchDialogProps {
  id: string;
  triggerButton?: React.ReactElement | undefined;
}

const DeletePatchDialog = ({ id, triggerButton }: DeletePatchDialogProps) => {
  const [open, setOpen] = React.useState(false);

  const queryClient = useQueryClient();
  const mutation = useMutation({
    ...deleteApiPatchFilesByIdMutation(),
    onSuccess: async () => {
      toast({
        title: "Successful!",
        description: "Entry has been deleted.",
      });
      setOpen(false);

      await queryClient.invalidateQueries({
        predicate: (query) =>
          // @ts-ignore
          query.queryKey[0]._id === getApiPatchFilesSummaryQueryKey()[0]._id,
      });
    },
  });

  return (
    <ConfirmationDialog
      open={open}
      onOpenChange={setOpen}
      icon="danger"
      title="Are you sure?"
      body={`This action cannot be undone.`}
      triggerButton={
        triggerButton ?? (
          <EnhancedButton variant={"destructive"}>Delete</EnhancedButton>
        )
      }
      confirmButton={
        <EnhancedButton
          className={"w-full"}
          effect={"expandIcon"}
          icon={LuTrash2}
          iconPlacement={"right"}
          variant={"destructive"}
          type="button"
          disabled={mutation.isPending}
          onClick={() => {
            mutation.mutate({
              path: {
                id: id,
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
          Delete
        </EnhancedButton>
      }
    />
  );
};

export default DeletePatchDialog;
