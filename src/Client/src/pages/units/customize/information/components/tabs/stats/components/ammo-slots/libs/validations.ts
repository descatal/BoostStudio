import {
  PostApiUnitStatsAmmoSlotRequest,
  UnitAmmoSlotDto,
  type CreateUnitAmmoSlotCommand,
  type PostApiUnitStatsAmmoSlotByIdRequest,
  type UpdateUnitAmmoSlotCommand,
} from "@/api/exvs"
import { z } from "zod"

export const updateAmmoSlotSchema = z.object({
  id: z.string(),
  ammoHash: z.number({ coerce: true }),
  unitId: z.number(),
  slotOrder: z.number(),
}) satisfies z.ZodType<UpdateUnitAmmoSlotCommand>

export type UpdateAmmoSlotSchema = z.infer<typeof updateAmmoSlotSchema>

export const createAmmoSlotSchema = z.object({
  ammoHash: z.number({ coerce: true }),
  unitId: z.number(),
  slotOrder: z.number(),
}) satisfies z.ZodType<CreateUnitAmmoSlotCommand>

export type CreateAmmoSlotSchema = z.infer<typeof createAmmoSlotSchema>
