import React, { useState } from "react";
import {
  FileUpload,
  FileUploadDropzone,
  FileUploadItem,
  FileUploadItemDelete,
  FileUploadItemMetadata,
  FileUploadItemPreview,
  FileUploadList,
  FileUploadTrigger,
} from "@/components/ui/file-upload";
import { Loader2, Upload, X } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { useMutation } from "@tanstack/react-query";
import { PostApiFhmPackRequest } from "@/api/exvs";
import { fhmApi } from "@/api/api";
import { toast } from "@/hooks/use-toast";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { LuPackage } from "react-icons/lu";
import { GetProblemDetails } from "@/lib/errors";

const formSchema = z.object({
  files: z
    .array(z.custom<File>())
    .min(1, "Please select at least one file")
    .max(1, "Please select only 1 file")
    .refine((files) => files.every((file) => file.size <= 5 * 1024 * 1024), {
      message: "File size must be less than 5MB",
      path: ["files"],
    }),
});

type FormValues = z.infer<typeof formSchema>;

const PackFhmForm = () => {
  const [packedFiles, setPackedFiles] = useState<File[]>([]);

  const form = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      files: [],
    },
  });

  const packMutation = useMutation({
    mutationFn: async (options: PostApiFhmPackRequest) => {
      toast({
        title: "Packing",
        description: "Packing the provided file into .fhm format...",
      });
      return fhmApi.postApiFhmPack(options);
    },
    onSuccess: (data) => {
      setPackedFiles([new File([data], "fileName")]);
      toast({
        title: "Pack successful!",
        description: "Packed FHM file successfully!",
      });
    },
    onSettled: async (_, error) => {
      if (error) {
        const problemDetails = await GetProblemDetails(error);
        form.setError("files", {
          message: problemDetails?.title ?? error.message,
        });
      }
    },
  });

  const onSubmit = React.useCallback((data: FormValues) => {
    packMutation.mutate({
      file: data.files[0],
    });
  }, []);

  return (
    <>
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="w-full">
          <FormField
            control={form.control}
            name="files"
            render={({ field, fieldState }) => (
              <FormItem>
                <FormLabel>Attachments</FormLabel>
                <FormControl>
                  <FileUpload
                    value={field.value}
                    onValueChange={field.onChange}
                    accept=".zip,.rar,.tar,.tar.gz"
                    maxFiles={1}
                    maxSize={100 * 1024 * 1024}
                    invalid={fieldState.invalid}
                    onFileReject={(_, message) => {
                      form.setError("files", {
                        message,
                      });
                    }}
                  >
                    <FileUploadDropzone>
                      <div className="flex flex-col items-center gap-1">
                        <div className="flex items-center justify-center rounded-full border p-2.5">
                          <Upload className="size-6 text-muted-foreground" />
                        </div>
                        <p className="font-medium text-sm">
                          Drag & drop files here
                        </p>
                        <p className="text-muted-foreground text-xs">
                          Or click to browse (max 1 files, up to 100MB each)
                        </p>
                      </div>
                      <FileUploadTrigger asChild>
                        <Button
                          variant="outline"
                          size="sm"
                          className="mt-2 w-fit"
                        >
                          Browse files
                        </Button>
                      </FileUploadTrigger>
                    </FileUploadDropzone>
                    <FileUploadList>
                      {field.value.map((file, index) => (
                        <FileUploadItem key={index} value={file}>
                          <FileUploadItemPreview />
                          <FileUploadItemMetadata />
                          <FileUploadItemDelete asChild>
                            <Button
                              variant="ghost"
                              size="icon"
                              className="size-7"
                            >
                              <X />
                            </Button>
                          </FileUploadItemDelete>
                        </FileUploadItem>
                      ))}
                    </FileUploadList>
                  </FileUpload>
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />
          <div className={"flex justify-end mt-3"}>
            <EnhancedButton
              disabled={packMutation.isPending}
              className={"w-1/12 min-w-fit"}
              icon={LuPackage}
              iconPlacement={"right"}
              effect={"expandIcon"}
            >
              {packMutation.isPending && (
                <Loader2 className="animate-spin mr-2" />
              )}
              Pack
            </EnhancedButton>
          </div>
        </form>
      </Form>
      <FileUpload>
        <FileUploadList>
          <FileUploadItem value={packedFiles[0]}>
            <FileUploadItemPreview />
            <FileUploadItemMetadata />
            <FileUploadItemDelete asChild>
              <Button variant="ghost" size="icon" className="size-7">
                <X />
              </Button>
            </FileUploadItemDelete>
          </FileUploadItem>
        </FileUploadList>
      </FileUpload>
    </>
  );
};

export default PackFhmForm;
