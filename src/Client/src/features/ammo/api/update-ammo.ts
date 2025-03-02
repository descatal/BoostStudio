import { ammoApi } from "@/api/api";
import { UpdateAmmoCommand } from "@/api/exvs";
import { useMutation, useQueryClient } from "@tanstack/react-query";

interface useUpdateAmmoMutationProps {
  onSuccess: () => void;
}

export const useUpdateAmmoMutation = ({
  onSuccess,
}: useUpdateAmmoMutationProps) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (options: UpdateAmmoCommand) =>
      ammoApi.postApiAmmoByHash({
        hash: options.hash,
        updateAmmoCommand: options,
      }),
    onSuccess: () => {
      void queryClient.invalidateQueries({
        queryKey: ["getApiStats"],
      });
      onSuccess();
    },
  });
};
