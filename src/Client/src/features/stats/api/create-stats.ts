import { hitboxesApi, statsApi } from "@/api/api";
import { CreateHitboxCommand, CreateStatCommand } from "@/api/exvs";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { MutationConfig } from "@/lib/react-query";
import { getHitboxGroupsOptions } from "@/features/hitboxes/api/get-hitbox-group";

interface useCreateStatsMutationProps {
  onSuccess: () => void;
}

export const useCreateStatsMutation = ({
  onSuccess,
}: useCreateStatsMutationProps) => {
  return useMutation({
    mutationFn: (options: CreateStatCommand) =>
      statsApi.postApiStats({ createStatCommand: options }),
    onSuccess: onSuccess,
  });
};

function createStats(options: CreateStatCommand) {
  return statsApi.postApiStats({ createStatCommand: options });
}

type UseCreateStatsOptions = {
  mutationConfig?: MutationConfig<typeof createStats>;
};

export const useCreateStats = ({ mutationConfig }: UseCreateStatsOptions) => {
  const queryClient = useQueryClient();

  const { onSuccess, ...restConfig } = mutationConfig || {};

  return useMutation({
    onSuccess: async (...args) => {
      await queryClient.invalidateQueries({
        queryKey: [],
      });
      onSuccess?.(...args);
    },
    ...restConfig,
    mutationFn: createStats,
  });
};
