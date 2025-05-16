import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { toast } from "sonner";
import { RefreshCcw } from "lucide-react";
import {
  TagsInput,
  TagsInputClear,
  TagsInputInput,
  TagsInputItem,
  TagsInputLabel,
  TagsInputList,
} from "@/components/ui/tags-input";
import { Button } from "@/components/ui/button";
import { TagsInputItemDelete, TagsInputItemText } from "@diceui/tags-input";
import { useMutation, useQuery } from "@tanstack/react-query";
import {
  getApiConfigsByKeyOptions,
  postApiConfigsMutation,
} from "@/api/exvs/@tanstack/react-query.gen.ts";
import { FaSave } from "react-icons/fa";
import { Icons } from "@/components/icons.tsx";

export const overlaySettingsFormSchema = z.object({
  windowTitles: z
    .array(z.string())
    .nonempty("Please enter at least one window title!"),
  interval: z.number().optional(),
});

export type OverlaySettings = z.infer<typeof overlaySettingsFormSchema>;

export const OVERLAY_SETTINGS = "OVERLAY_SETTINGS";

const OverlayAdvancedSettingsForm = () => {
  const query = useQuery({
    ...getApiConfigsByKeyOptions({
      path: {
        key: OVERLAY_SETTINGS,
      },
    }),
    select: (data) => {
      return JSON.parse(data) as OverlaySettings;
    },
    retry: 1,
  });

  const mutation = useMutation({
    ...postApiConfigsMutation(),
  });

  const form = useForm<OverlaySettings>({
    resolver: zodResolver(overlaySettingsFormSchema),
    values: {
      windowTitles: query.data?.windowTitles ?? ["NPJB00512", "BLJS10250"],
      interval: query.data?.interval ?? 300,
    },
  });

  async function onSubmit(values: OverlaySettings) {
    try {
      const jsonPayload = JSON.stringify(values, null, 2);
      await mutation.mutateAsync({
        body: {
          key: OVERLAY_SETTINGS,
          value: jsonPayload,
        },
      });
      await query.refetch();
      form.reset();

      toast(
        <div>
          <p>The following values have been saved into config:</p>
          <pre className="mt-2 w-[340px] rounded-md bg-slate-950 p-4">
            <code className="text-white">{jsonPayload}</code>
          </pre>
        </div>,
      );
    } catch (error) {
      console.error("Form submission error", error);
      toast.error("Failed to submit the form. Please try again.");
    }
  }

  if (query.error) {
    return <p>Failed to retrieve settings!</p>;
  }
  if (query.isLoading) return <p>Loading..</p>;

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="space-y-8 max-w-3xl mx-auto"
      >
        <FormField
          control={form.control}
          name="windowTitles"
          render={({ field }) => (
            <FormItem>
              <TagsInput
                value={field.value}
                onValueChange={field.onChange}
                className={"w-full"}
                editable
              >
                <TagsInputLabel>
                  <FormLabel>Window Titles</FormLabel>
                </TagsInputLabel>
                <FormControl>
                  <TagsInputList>
                    {field.value.map((fieldName) => (
                      <TagsInputItem key={fieldName} value={fieldName}>
                        <TagsInputItemText />
                        <TagsInputItemDelete />
                      </TagsInputItem>
                    ))}
                    <TagsInputInput
                      placeholder="Add window title..."
                      className={"w-fit"}
                    />
                  </TagsInputList>
                </FormControl>
                <TagsInputClear asChild>
                  <Button variant="outline">
                    <RefreshCcw className="h-4 w-4" />
                    Clear
                  </Button>
                </TagsInputClear>
              </TagsInput>
              <FormDescription>
                The window titles to search for the listener to attach to.
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />
        <EnhancedButton
          type="submit"
          className={"w-full"}
          icon={FaSave}
          iconPlacement={"right"}
        >
          {mutation.isPending && (
            <Icons.spinner
              className="size-4 mr-2 animate-spin"
              aria-hidden="true"
            />
          )}
          Submit
        </EnhancedButton>
      </form>
    </Form>
  );
};

export default OverlayAdvancedSettingsForm;
