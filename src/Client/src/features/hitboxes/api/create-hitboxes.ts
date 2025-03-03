import { hitboxesApi } from "@/api/api";
import { CreateHitboxCommand } from "@/api/exvs";
import { useMutation } from "@tanstack/react-query";

interface useApiCreateHitboxesMutationProps {
  onSuccess: () => void;
}

export const useApiCreateHitboxesMutation = ({
  onSuccess,
}: useApiCreateHitboxesMutationProps) => {
  return useMutation({
    mutationFn: (options: CreateHitboxCommand) =>
      hitboxesApi.postApiHitboxes({ createHitboxCommand: options }),
    onSuccess: onSuccess,
  });
};
