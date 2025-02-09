import { z } from "zod"

export const exportDialogSchema = z.object({
  unitId: z.coerce.number().optional(),
  stats: z.boolean().optional(),
  ammo: z.boolean().optional(),
  projectile: z.boolean().optional(),
  hitbox: z.boolean().optional(),
})

export type ExportDialogSchema = z.infer<typeof exportDialogSchema>
