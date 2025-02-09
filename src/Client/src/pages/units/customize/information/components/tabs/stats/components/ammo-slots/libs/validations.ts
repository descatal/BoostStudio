import { z } from "zod";
import {type PostApiUnitStatsAmmoSlotByIdRequest, UnitAmmoSlotDto, PostApiUnitStatsAmmoSlotRequest} from "@/api/exvs";

export const updateAmmoSlotSchema = z.object({
  id: z.string(),
  ammoHash: z.number({coerce: true}),
  unitId: z.number(),
  slotOrder: z.number(),
}) satisfies z.ZodType<PostApiUnitStatsAmmoSlotByIdRequest>

export type UpdateAmmoSlotSchema = z.infer<typeof updateAmmoSlotSchema>

export const createAmmoSlotSchema = z.object({
  ammoHash: z.number({coerce: true}),
  unitId: z.number(),
  slotOrder: z.number(),
}) satisfies z.ZodType<PostApiUnitStatsAmmoSlotRequest>

export type CreateAmmoSlotSchema = z.infer<typeof createAmmoSlotSchema>