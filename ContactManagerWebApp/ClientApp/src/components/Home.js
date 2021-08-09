import React, { Component } from 'react';

import DataGrid, { Column, Pager, Paging, Editing } from 'devextreme-react/data-grid';
//import DataSource from 'devextreme/data/data_source';
import CustomStore from 'devextreme/data/custom_store';

const URL = process.env.REACT_APP_API_SERVER_URL + '/api/ContactManager';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        this.state = {
            //contacts: [],
            //loading: true,
            contactsData: new CustomStore({
                key: 'personId',
                load: () => this.sendRequest(`${URL}/Contacts`),
                insert: (values) => this.sendRequest(`${URL}/InsertContact`, 'POST', values),
                update: (key, values) => this.sendRequest(`${URL}/UpdateContact/${key}`, 'PUT', values),
                remove: (key) => this.sendRequest(`${URL}/DeleteContact/${key}`, 'DELETE')
            })
        };

        //this.dataSource = new DataSource({
        //    store: {
        //        type: 'array',
        //        data: items
        //    },
        //    sort: { getter: 'last' }
        //});
    }

    //componentDidMount() {
    //    this.getContactData();
    //}

    //static renderContactTable(contacts) {
    //    return (
    //        <table className='table table-striped' aria-labelledby="tabelLabel">
    //            <thead>
    //                <tr>
    //                    <th>Type</th>
    //                    <th>First Name</th>
    //                    <th>Last Name</th>
    //                    <th>Email</th>
    //                    <th>Phone</th>
    //                    <th>Birthday</th>
    //                </tr>
    //            </thead>
    //            <tbody>
    //                {contacts.items.map(contact =>
    //                    <tr key={contact.personId}>
    //                        <td>{contact.type}</td>
    //                        <td>{contact.first}</td>
    //                        <td>{contact.last}</td>
    //                        <td>{contact.email}</td>
    //                        <td>{contact.telephone}</td>
    //                        <td>{contact.birthday ? new Date(contact.birthday).toLocaleDateString() : ''}</td>
    //                    </tr>
    //                )}
    //            </tbody>
    //        </table>
    //    );
    //}

    //componentWillUnmount() {
    //    // A DataSource instance created outside a UI component should be disposed of manually.
    //    this.dataSource.dispose();
    //}

    //async getContactData() {
    //    console.log("URL", URL);

    //    const response = await fetch(`${URL}/Contacts?Limit=20&Page=1`);
    //    const data = await response.json();
    //    console.log("data", data);
    //    this.setState({ contacts: data, loading: false });
    //}

    sendRequest(url, method, data) {
        method = method || 'GET';
        data = data || {};

        console.log("sendRequest url", url);

        if (method === 'GET') {
            return fetch(url, {
                method: method
            }).then(result => result.json().then(json => {
                console.log("sendRequest", json);
                if (result.ok) return json.items;
                throw json.Message;
            }));
        }

        console.log("sendRequest data", JSON.stringify(data));

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

    render() {
        const { contactsData } = this.state;

        //let contents = this.state.loading
        //    ? <p><em>Loading...</em></p>
        //    : Home.renderContactTable(this.state.contacts);

        return (
            <div>
                <DataGrid
                    dataSource={contactsData}
                    showBorders={true}
                    showRowLines={true}
                    rowAlternationEnabled={true}
                >
                    <Editing
                        refreshMode="full"
                        mode="cell"
                        allowAdding={true}
                        allowDeleting={true}
                        allowUpdating={true}
                    />
                    <Column dataField="type"></Column>
                    <Column dataField="first"></Column>
                    <Column dataField="last"></Column>
                    <Column dataField="email"></Column>
                    <Column dataField="telephone"></Column>
                    <Column dataField="birthday" dataType="date"></Column>
                    <Paging defaultPageSize={2} />
                    <Pager
                        visible={true}
                        allowedPageSizes={[2, 5]}
                        displayMode="compact"
                        showPageSizeSelector={true}
                        showNavigationButtons={true} />
                </DataGrid>
            </div>
        );
    }

}
