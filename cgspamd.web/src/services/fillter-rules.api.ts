import {queryClient} from "../main.tsx";
import {AddData, DeleteData, FetchData, UpdateData} from "./DataService.api.ts";
const filterRulesApiUrl = import.meta.env.VITE_FILTER_RULES_API_URL;

export const addRule = async (body:string)=>{
  const res = await AddData(filterRulesApiUrl, body);
  if (res.status !== 201) {
    throw res.status;
  }
  return res.json();
}
export const updateRule = async (body:string)=>{
  const res = await UpdateData(filterRulesApiUrl, body);
  if (res.status !== 200) {
    throw res.status;
  }
  return res.json();
};
export const deleteRule = async (id:number)=>{
  const res = await DeleteData(`${filterRulesApiUrl}/${id}`);
  if (res.status !== 204) {
    throw res.status;
  }
}
export const loadAllRules = async () => {
  return await queryClient.fetchQuery({
    queryKey: ["rules", "get"],
    queryFn:()=> FetchData(filterRulesApiUrl)
      .then(res => {
        if (res.status !== 200) {
          throw res.status;
        }
        return res.json();
      }),
    staleTime: 60_000,
  });
};
