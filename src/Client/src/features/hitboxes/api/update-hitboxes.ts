import { hitboxesApi } from "@/api/api";
import { UpdateHitboxCommand } from "@/api/exvs";
import { useMutation, useQueryClient } from "@tanstack/react-query";

interface useApiUpdateHitboxesMutationProps {
  onSuccess: () => void;
}

export const useApiUpdateHitboxesMutation = ({
  onSuccess,
}: useApiUpdateHitboxesMutationProps) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (options: UpdateHitboxCommand) =>
      hitboxesApi.postApiHitboxesByHash({
        hash: options.hash,
        updateHitboxCommand: options,
      }),
    onSuccess: () => {
      void queryClient.invalidateQueries({
        queryKey: ["getApiHitboxes", "getApiHitboxGroups"],
      });
      onSuccess();
    },
  });
};
