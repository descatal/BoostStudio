import { projectilesApi } from "@/api/api";
import { UpdateProjectileByIdCommand } from "@/api/exvs";
import { useMutation, useQueryClient } from "@tanstack/react-query";

interface useApiUpdateProjectilesMutationProps {
  onSuccess: () => void;
}

export const useApiUpdateProjectilesMutation = ({
  onSuccess,
}: useApiUpdateProjectilesMutationProps) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (options: UpdateProjectileByIdCommand) =>
      projectilesApi.postApiProjectilesByHash({
        hash: options.hash,
        updateProjectileByIdCommand: options,
      }),
    onSuccess: () => {
      void queryClient.invalidateQueries({
        queryKey: ["getApiProjectiles", "getApiUnitProjectiles"],
      });
      onSuccess();
    },
  });
};
