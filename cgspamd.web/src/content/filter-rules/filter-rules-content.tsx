import {observer} from "mobx-react";
import {useEffect} from "react";
import {reaction} from "mobx";
import {rootStore} from "@/store/root-store.ts";
import {filterRuleStore} from "@/content/filter-rules/filter-rule-store.ts";
import {DataTable} from "@/components/data-table.tsx";
import {filterRuleColumns} from "@/content/filter-rules/filter-rules-columns.tsx";
import {FilterRuleGlobalActions} from "@/content/filter-rules/components/filter-rule-global-actions.tsx";

export const FilterRulesContent = observer(() => {
  useEffect(()=>{
    return   reaction(
      ()=>rootStore.isLoggedIn,
      ()=>{
        if (rootStore.isLoggedIn){
          filterRuleStore.LoadAllRules().then();
        }
      },
      { fireImmediately: true }
    );
  },[]);
  return (
    <section className="container mx-auto py-10">
      <DataTable
        columns={filterRuleColumns}
        data={filterRuleStore.filterRulesData}
      />
      <div className="absolute top-4 left-5">
        <FilterRuleGlobalActions />
      </div>
    </section>
  );
})