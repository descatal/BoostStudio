import { hitboxesApi } from "@/api/api";
import { CreateHitboxCommand } from "@/api/exvs";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { MutationConfig } from "@/lib/react-query";
import { getHitboxGroupsOptions } from "@/features/hitboxes/api/get-hitbox-group";

function createHitboxes(options: CreateHitboxCommand) {
  return hitboxesApi.postApiHitboxes({ createHitboxCommand: options });
}

type UseCreateHitboxesOptions = {
  mutationConfig?: MutationConfig<typeof createHitboxes>;
};

export const useCreateHitboxes = ({
  mutationConfig,
}: UseCreateHitboxesOptions) => {
  const queryClient = useQueryClient();

  const { onSuccess, ...restConfig } = mutationConfig || {};

  return useMutation({
    onSuccess: async (...args) => {
      await queryClient.invalidateQueries({
        queryKey: getHitboxGroupsOptions({}).queryKey,
      });
      onSuccess?.(...args);
    },
    ...restConfig,
    mutationFn: createHitboxes,
  });
};
