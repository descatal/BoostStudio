import { hitboxesApi } from "@/api/api";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { MutationConfig } from "@/lib/react-query";
import { getHitboxGroupsOptions } from "@/features/hitboxes/api/get-hitbox-group";
import { getHitboxesOptions } from "@/features/hitboxes/api/get-hitboxes";
import { UpdateHitboxCommand } from "@/api/exvs";

function updateHitbox(options: UpdateHitboxCommand) {
  return hitboxesApi.postApiHitboxesByHash({
    hash: options.hash,
    updateHitboxCommand: options,
  });
}

type UseUpdateHitboxesOptions = {
  mutationConfig?: MutationConfig<typeof updateHitbox>;
};

export const useUpdateHitboxes = ({
  mutationConfig,
}: UseUpdateHitboxesOptions) => {
  const queryClient = useQueryClient();

  const { onSuccess, ...restConfig } = mutationConfig || {};

  return useMutation({
    onSuccess: async (...args) => {
      await queryClient.invalidateQueries({
        queryKey: [
          getHitboxesOptions({}).queryKey,
          getHitboxGroupsOptions({}).queryKey,
        ],
      });
      onSuccess?.(...args);
    },
    ...restConfig,
    mutationFn: updateHitbox,
  });
};
