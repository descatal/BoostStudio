import {
  CreatePatchFileCommand,
  FileInfoDto,
  PatchFileVersion,
  type PathInfoDto,
  type UpdatePatchFileByIdCommand,
} from "@/api/exvs"
import { z } from "zod"

const patchFileVersionEnum = z.nativeEnum(PatchFileVersion)

const pathInfoSchema = z.object({
  path: z.string(),
  order: z.coerce.number().optional(),
}) satisfies z.ZodType<PathInfoDto>

const fileInfoSchema = z.object({
  version: patchFileVersionEnum,
  size1: z.coerce.number(),
  size2: z.coerce.number(),
  size3: z.coerce.number(),
  size4: z.coerce.number(),
}) satisfies z.ZodType<FileInfoDto>

export const createPatchFileSchema = z
  .object({
    tblId: patchFileVersionEnum,
    pathInfo: pathInfoSchema.optional(),
    fileInfo: fileInfoSchema.optional(),
    assetFileHash: z.coerce.number().optional(),
  })
  .superRefine((object, ctx) => {
    if (object.fileInfo && !object.assetFileHash) {
      ctx.addIssue({
        path: ["assetFileHash"],
        code: z.ZodIssueCode.custom,
        message: "Asset file hash is required",
      })
    }

    if (!object.fileInfo) {
      object.assetFileHash = undefined
    }
  }) satisfies z.ZodType<CreatePatchFileCommand>

export type CreatePatchFileSchema = z.infer<typeof createPatchFileSchema>

export const updatePatchFileSchema = z
  .object({
    id: z.string(),
    tblId: patchFileVersionEnum,
    pathInfo: pathInfoSchema.optional(),
    fileInfo: fileInfoSchema.optional(),
    assetFileHash: z.coerce.number().optional(),
  })
  .superRefine((object) => {
    if (!object.fileInfo) {
      object.assetFileHash = undefined
    }
  }) satisfies z.ZodType<UpdatePatchFileByIdCommand>

export type UpdatePatchFileSchema = z.infer<typeof updatePatchFileSchema>

export interface AssetFileSearch {
  assetFileHash: number
}

export const searchAssetFileSearchSchema = z.object({
  assetFileHash: z.number(),
}) satisfies z.ZodType<AssetFileSearch>

export type SearchAssetFileSchema = z.infer<typeof searchAssetFileSearchSchema>
