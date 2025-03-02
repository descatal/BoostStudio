import { statsApi } from "@/api/api";
import { type UpdateStatCommand } from "@/api/exvs";
import { useMutation, useQueryClient } from "@tanstack/react-query";

interface useUpdateStatsMutationProps {
  onSuccess: () => void;
}

export const useUpdateStatsMutation = ({
  onSuccess,
}: useUpdateStatsMutationProps) => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (options: UpdateStatCommand) =>
      statsApi.postApiStatsById({ id: options.id, updateStatCommand: options }),
    onSuccess: () => {
      void queryClient.invalidateQueries({
        queryKey: ["getApiStats"],
      });
      onSuccess();
    },
  });
};
