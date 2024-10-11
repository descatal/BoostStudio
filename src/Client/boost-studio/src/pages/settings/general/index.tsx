import React, { useCallback, useEffect } from "react"
import { UpsertConfigCommand } from "@/api/exvs"
import { fetchConfigs, upsertConfig } from "@/api/wrapper/config-api"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import { z } from "zod"

import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { toast } from "@/components/ui/use-toast"

import { useSettingsStore } from "../libs/store"

const STAGING_DIRECTORY = "STAGING_DIRECTORY"
const WORKING_DIRECTORY = "WORKING_DIRECTORY"

const upsertConfigSchema = z.object({
  key: z.string(),
  value: z.string(),
}) satisfies z.ZodType<UpsertConfigCommand>

const General = () => {
  const store = useSettingsStore()

  const getConfigs = useCallback(async () => {
    const configs = await fetchConfigs({
      keys: [STAGING_DIRECTORY, WORKING_DIRECTORY],
    })

    const stagingDirectory = configs.find(
      (config) => config.key === STAGING_DIRECTORY
    )?.value
    const workingDirectory = configs.find(
      (config) => config.key === WORKING_DIRECTORY
    )?.value

    store.updateWorkingDirectory(workingDirectory)
    store.updateStagingDirectory(stagingDirectory)

    if (stagingDirectory)
      stagingDirectoryForm.setValue("value", stagingDirectory)
    if (workingDirectory)
      workingDirectoryForm.setValue("value", workingDirectory)
  }, [])

  const saveConfig = useCallback(async (Key: string, Value: string) => {
    await upsertConfig({
      upsertConfigCommand: {
        key: Key,
        value: Value,
      },
    })

    toast({
      title: "Saved config successfully!",
    })
  }, [])

  useEffect(() => {
    getConfigs().catch((err) => console.log(err))
  }, [])

  const stagingDirectoryForm = useForm<z.infer<typeof upsertConfigSchema>>({
    resolver: zodResolver(upsertConfigSchema),
    defaultValues: {
      key: STAGING_DIRECTORY,
      value: "",
    },
  })

  const workingDirectoryForm = useForm<z.infer<typeof upsertConfigSchema>>({
    resolver: zodResolver(upsertConfigSchema),
    defaultValues: {
      key: WORKING_DIRECTORY,
      value: "",
    },
  })

  const onConfigFormSubmit = async (
    values: z.infer<typeof upsertConfigSchema>
  ) => {
    await saveConfig(values.key, values.value)

    if (values.key === STAGING_DIRECTORY) {
      store.updateStagingDirectory(values.value)
    } else if (values.key === WORKING_DIRECTORY) {
      store.updateWorkingDirectory(values.value)
    }
  }

  return (
    <div className="grid gap-6">
      <Form {...stagingDirectoryForm}>
        <form
          onSubmit={stagingDirectoryForm.handleSubmit(onConfigFormSubmit)}
          className={"space-y-8"}
        >
          <Card x-chunk="dashboard-04-chunk-1">
            <CardHeader>
              <CardTitle>Staging Directory</CardTitle>
              <CardDescription>
                The `.moddedboost` folder inside your RPCS3 directory
              </CardDescription>
            </CardHeader>
            <CardContent>
              <FormField
                control={stagingDirectoryForm.control}
                name={"value"}
                render={({ field }) => (
                  <FormItem>
                    <FormControl>
                      <Input placeholder="/rpcs3/.moddedboost" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </CardContent>
            <CardFooter className="border-t px-6 py-4">
              <Button type={"submit"} variant={"default"}>
                Save
              </Button>
            </CardFooter>
          </Card>
        </form>
      </Form>
      <Form {...workingDirectoryForm}>
        <form
          onSubmit={workingDirectoryForm.handleSubmit(onConfigFormSubmit)}
          className={"space-y-8"}
        >
          <Card x-chunk="dashboard-04-chunk-2">
            <CardHeader>
              <CardTitle>Working Directory</CardTitle>
              <CardDescription>
                The working directory to store intermediate files.
              </CardDescription>
            </CardHeader>
            <CardContent>
              <FormField
                control={workingDirectoryForm.control}
                name={"value"}
                render={({ field }) => (
                  <FormItem>
                    <FormControl>
                      <Input placeholder="/workstation" {...field} />
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </CardContent>
            <CardFooter className="border-t px-6 py-4">
              <Button type={"submit"}>Save</Button>
            </CardFooter>
          </Card>
        </form>
      </Form>
    </div>
  )
}

export default General
