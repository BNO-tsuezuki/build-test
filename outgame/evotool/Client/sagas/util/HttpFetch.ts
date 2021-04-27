
export default function (uri: string, payload: Object, orgHeaders: Object = {}) {

  let headers = Object.assign({ "Content-Type": "application/json" }, orgHeaders);

  return fetch(
    uri,
    {
      method: "POST",
      headers: headers,
      body: JSON.stringify(payload)
    }
  )
  .then(res => {
    if (res.ok) return res.json();
    return { status: "error", error: { msg: res.status + ":" + res.statusText } };
  })
  .catch(error => {
    return { status: "error", error: { msg:error.message } };
  });
  
}
