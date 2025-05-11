import React from "react";
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
import { RefreshCcw, X } from "lucide-react";
import {
  TagsInputClear,
  TagsInputInput,
  TagsInputItem,
  TagsInputItemDelete,
  TagsInputItemText,
  TagsInputLabel,
} from "@diceui/tags-input";
import { TagsInput, TagsInputList } from "@/components/ui/tags-input";
import { Button } from "@/components/ui/button";

const formSchema = z.object({
  windowTitles: z
    .array(z.string())
    .nonempty("Please enter at least one window title!"),
  interval: z.number().optional(),
});

const OverlayAdvancedSettingsForm = () => {
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      windowTitles: ["NPJB00512", "BLJS10250"],
      interval: 300,
    },
  });

  function onSubmit(values: z.infer<typeof formSchema>) {
    try {
      toast(
        <pre className="mt-2 w-[340px] rounded-md bg-slate-950 p-4">
          <code className="text-white">{JSON.stringify(values, null, 2)}</code>
        </pre>,
      );
    } catch (error) {
      console.error("Form submission error", error);
      toast.error("Failed to submit the form. Please try again.");
    }
  }

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
              >
                <TagsInputLabel>
                  <FormLabel>Window Titles</FormLabel>
                </TagsInputLabel>
                <FormControl>
                  <TagsInputList>
                    {field.value.map((fieldName) => (
                      <TagsInputItem key={fieldName} value={fieldName}>
                        <TagsInputItemText />
                        <TagsInputItemDelete>
                          <X className="h-3.5 w-3.5" />
                        </TagsInputItemDelete>
                      </TagsInputItem>
                    ))}
                    <TagsInputInput
                      placeholder="Add window title..."
                      className={"w-full"}
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
        <EnhancedButton type="submit">Submit</EnhancedButton>
      </form>
    </Form>
  );
};

export default OverlayAdvancedSettingsForm;
