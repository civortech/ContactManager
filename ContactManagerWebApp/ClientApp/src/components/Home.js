import React, { Component } from 'react';

import DataGrid, { Column, Pager, Paging, Editing, Lookup, EmailRule, RequiredRule, PatternRule } from 'devextreme-react/data-grid';
//import DataSource from 'devextreme/data/data_source';
import CustomStore from 'devextreme/data/custom_store';

const URL = process.env.REACT_APP_API_SERVER_URL + '/api/ContactManager';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        this.datagridRef = React.createRef();

        this.state = {
            contactsData: new CustomStore({
                key: ['personId', 'contactId', 'type'],
                load: () => this.sendRequest(`${URL}/Contacts`),
                insert: (values) => this.sendRequest(`${URL}/InsertContact`, 'POST', values),
                update: (key, values) => this.sendRequest(`${URL}/UpdateContact/${key.type}/${key.personId}`, 'PUT', values),
                remove: (key) => {
                    console.log("remove key", key);
                    return this.sendRequest(`${URL}/DeleteContact/${key.type}/${key.personId}`, 'DELETE');
                }
            })
        };

    }

    sendRequest(url, method, data) {
        method = method || 'GET';
        data = data || {};
        console.log("sendRequest method url data", method, url, JSON.stringify(data));

        if (method === 'GET') {
            return fetch(url, {
                method: method
            }).then(result => result.json().then(json => {
                console.log("sendRequest", json);
                if (result.ok) return json.items;
                throw json.Message;
            }));
        }

        return fetch(url, {
            method: method,
            body: JSON.stringify(data),
            headers: {
                'Content-Type': 'application/json',
                'accept': '*'
            }
        }).then(result => {
            if (result.ok) {
                return result.text().then(text => text && JSON.parse(text));
            } else {
                return result.json().then(json => {
                    throw json.Message;
                });
            }
        });
    }

    overrideOnEditorPreparing(e) {
        console.log("overrideOnEditorPreparing e", e);
        if (e.dataField === 'type' && e.parentType === 'dataRow') {
            if (e.row.rowType === "data") {
                e.editorOptions.disabled = !e.row.isNewRow;
            } else {
                e.editorOptions.disabled = false;
            }
        }
    }

    setTypeValue(rowData, value) {
        console.log("setTypeValue rowData value", rowData, value);
        if (value === 'c') {
            rowData.telephone = null;
        } else {
            rowData.birthday = null;
            rowData.email = null;
        }
        this.defaultSetCellValue(rowData, value);
    }

    render() {
        const { contactsData } = this.state;

        return (
            <div>
                <DataGrid
                    ref={this.datagridRef}
                    dataSource={contactsData}
                    showBorders={true}
                    showRowLines={true}
                    rowAlternationEnabled={true}
                    onEditorPreparing={this.overrideOnEditorPreparing}
                >
                    <Editing
                        refreshMode="full"
                        mode="popup"
                        allowAdding={true}
                        allowDeleting={true}
                        allowUpdating={true}
                    />
                    <Column
                        dataField="type"
                        setCellValue={this.setTypeValue}
                    >
                        <Lookup
                            dataSource={[{ 'type': 'c', 'desc': 'Customer' }, { 'type': 's', 'desc': 'Supplier' }]}
                            valueExpr="type"
                            displayExpr="desc" />
                        <RequiredRule />
                    </Column>
                    <Column
                        dataField="first"
                        caption="First Name"
                    >
                        <PatternRule
                            message={'First Name must be up to 50 characters!'}
                            pattern={/^[a-zA-Z0-9]{1,50}$/}
                        /> 
                    </Column>
                    <Column
                        dataField="last"
                        caption="Last Name"
                    >
                        <PatternRule
                            message={'Last Name must be up to 50 characters!'}
                            pattern={/^[a-zA-Z0-9]{1,50}$/}
                        />
                    </Column>
                    <Column dataField="email">
                        <EmailRule />
                    </Column>
                    <Column dataField="telephone">
                        <PatternRule
                            message={'Phone must be between 7 and 12 numeric values!'}
                            pattern={/^\d{7,12}$/}
                        />
                    </Column>
                    <Column dataField="birthday" dataType="date"></Column>
                    <Paging defaultPageSize={5} />
                    <Pager
                        visible={true}
                        allowedPageSizes={[5, 10]}
                        displayMode="compact"
                        showPageSizeSelector={true}
                        showNavigationButtons={true} />
                </DataGrid>
            </div>
        );
    }

}
