import {
  CreatePatchFileCommand,
  FileInfoDto,
  type PathInfoDto,
  type UpdatePatchFileByIdCommand,
} from "@/api/exvs";
import { z } from "zod";
import {
  zCreatePatchFileCommand,
  zPatchFileVersion,
  zUpdatePatchFileByIdCommand,
} from "@/api/exvs/zod.gen";

const patchFileVersionEnum = z.nativeEnum(zPatchFileVersion.Enum);

const pathInfoSchema = z.object({
  path: z.string(),
  order: z.coerce.number().nullable(),
}) satisfies z.ZodType<PathInfoDto>;

const fileInfoSchema = z.object({
  version: patchFileVersionEnum,
  size1: z.coerce.number(),
  size2: z.coerce.number(),
  size3: z.coerce.number(),
  size4: z.coerce.number(),
}) satisfies z.ZodType<FileInfoDto>;

export const createPatchFileSchema = z
  .object({
    tblId: patchFileVersionEnum,
    pathInfo: pathInfoSchema.optional().nullable(),
    fileInfo: fileInfoSchema.optional().nullable(),
    assetFileHash: z.coerce.number().nullable(),
  })
  .superRefine((object, ctx) => {
    if (object.fileInfo && !object.assetFileHash) {
      ctx.addIssue({
        path: ["assetFileHash"],
        code: z.ZodIssueCode.custom,
        message: "Asset file hash is required",
      });
    }

    if (!object.fileInfo) {
      object.assetFileHash = null;
    }
  }) satisfies z.ZodType<CreatePatchFileCommand>;

// export type CreatePatchFileSchema = z.infer<typeof createPatchFileSchema>;

export const updatePatchFileSchema = z
  .object({
    id: z.string(),
    tblId: patchFileVersionEnum,
    pathInfo: pathInfoSchema.optional().nullable(),
    fileInfo: fileInfoSchema.optional().nullable(),
    assetFileHash: z.coerce.number().nullable(),
  })
  .superRefine((object) => {
    if (!object.fileInfo) {
      object.assetFileHash = null;
    }
  }) satisfies z.ZodType<UpdatePatchFileByIdCommand>;

// export type UpdatePatchFileSchema = z.infer<typeof updatePatchFileSchema>;

export interface AssetFileSearch {
  assetFileHash: number;
}

export const searchAssetFileSearchSchema = z.object({
  assetFileHash: z.number(),
}) satisfies z.ZodType<AssetFileSearch>;

export type SearchAssetFileSchema = z.infer<typeof searchAssetFileSearchSchema>;

export type CreatePatchFileSchema = z.infer<typeof zCreatePatchFileCommand>;
export type UpdatePatchFileSchema = z.infer<typeof zUpdatePatchFileByIdCommand>;
