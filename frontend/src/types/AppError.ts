export type AppError = {
    detail: string,
    status: number,
    validationErrorEntries: Array<AppErrorEntry>
}
export type AppErrorEntry = {
    field: string,
    message: string
}

export default function groupValidationEntriesByField(validationEntries: Array<AppErrorEntry>) : Map<string, Array<AppErrorEntry>>{
    let newErros : Map<string, Array<AppErrorEntry>> = new Map()
    validationEntries.forEach(value=>{
        if(newErros.has(value.field)){
            newErros.get(value.field)!.push(value)
        }
        else{
            newErros.set(value.field, [value])
        }
    })
    return newErros
}