import React, { useCallback, useEffect } from "react"
import { fetchConfigs } from "@/api/wrapper/config-api"

import { useSettingsStore } from "../libs/store"
import ConfigForm from "./forms/config-form"

const STAGING_DIRECTORY = "STAGING_DIRECTORY"
const WORKING_DIRECTORY = "WORKING_DIRECTORY"
const PRODUCTION_DIRECTORY = "PRODUCTION_DIRECTORY"
const SCRIPT_DIRECTORY = "SCRIPT_DIRECTORY"

const General = () => {
  const store = useSettingsStore()

  const getConfigs = useCallback(async () => {
    const configs = await fetchConfigs({
      keys: [
        STAGING_DIRECTORY,
        WORKING_DIRECTORY,
        PRODUCTION_DIRECTORY,
        SCRIPT_DIRECTORY,
      ],
    })

    const stagingDirectory = configs.find(
      (config) => config.key === STAGING_DIRECTORY
    )?.value
    const workingDirectory = configs.find(
      (config) => config.key === WORKING_DIRECTORY
    )?.value
    const productionDirectory = configs.find(
      (config) => config.key === PRODUCTION_DIRECTORY
    )?.value
    const scriptDirectory = configs.find(
      (config) => config.key === SCRIPT_DIRECTORY
    )?.value

    store.updateWorkingDirectory(workingDirectory)
    store.updateStagingDirectory(stagingDirectory)
    store.updateProductionDirectory(productionDirectory)
    store.updateScriptDirectory(scriptDirectory)
  }, [])

  useEffect(() => {
    getConfigs().catch((err) => console.log(err))
  }, [])

  const onConfigFormSubmit = async (key: string, value: string) => {
    if (key === STAGING_DIRECTORY) {
      store.updateStagingDirectory(value)
    } else if (key === WORKING_DIRECTORY) {
      store.updateWorkingDirectory(value)
    } else if (key === PRODUCTION_DIRECTORY) {
      store.updateProductionDirectory(value)
    } else if (key === SCRIPT_DIRECTORY) {
      store.updateScriptDirectory(value)
    }
  }

  return (
    <div className="grid gap-6">
      <ConfigForm
        title={"Staging Directory"}
        description={"The `.moddedboost` folder inside your RPCS3 directory"}
        placeholder={"/rpcs3/.moddedboost"}
        configKey={STAGING_DIRECTORY}
        configValue={store.stagingDirectory}
        onSubmit={onConfigFormSubmit}
      />
      <ConfigForm
        title={"Working Directory"}
        description={"The working directory to store intermediate asset files."}
        placeholder={"/workstation"}
        configKey={WORKING_DIRECTORY}
        configValue={store.workingDirectory}
        onSubmit={onConfigFormSubmit}
      />
      <ConfigForm
        title={"Production Directory"}
        description={"The directory that stores packed psarc files."}
        placeholder={"/rpcs3/dev_hdd0/game/NPJB00512/USRDIR"}
        configKey={PRODUCTION_DIRECTORY}
        configValue={store.productionDirectory}
        onSubmit={onConfigFormSubmit}
      />
      <ConfigForm
        title={"Script Directory"}
        description={"The directory that stores units' scex script files."}
        placeholder={"/scripts"}
        configKey={SCRIPT_DIRECTORY}
        configValue={store.scriptDirectory}
        onSubmit={onConfigFormSubmit}
      />
    </div>
  )
}

export default General
