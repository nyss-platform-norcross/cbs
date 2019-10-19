import React from 'react';
import moment from 'moment';
import axios from 'axios';
import Timeline from 'react-calendar-timeline'
// import Timeline, {
//   TimelineHeaders,
//   SidebarHeader,
//   DateHeader
// } from 'react-calendar-timeline'
import 'react-calendar-timeline/lib/Timeline.css'

import './AlertHistoryView.css'
import AlertHistoryContainer from '../../components/alert/alert-history-container/AlertHistoryContainer'

class AlertHistoryView extends React.Component {

  state = {
    data: [],
    dateRange: {
      startDate: null,
      endDate: null
    }
  }

  getParsedDataByMonth = item => {
    console.log('data', item)

    const monthsList = moment.monthsShort()
    const dateStart = moment('2013-8-31');
    const dateEnd = moment('2015-3-30');

    const timeValues = [];

    while (dateEnd > dateStart || dateStart.format('M') === dateEnd.format('M')) {
      timeValues.push(dateStart.format('YYYY-MM'));
      dateStart.add(1,'month');
    }
    monthsList.forEach(month => {
      console.log('month', month)
    })
    // console.log(monthsList)



    return 'test'
  }

  getListItemsByVillage = data => {
    const villages = data.villages.map(item => {
      return {
        itemLabel: item.village,
        data: item.alerts
      }
    })
    return villages
  }

  componentDidMount = async () => {
    const { data } = await axios.get("http://localhost:5000/api/AlertHistory?numberOfWeeks=52&includeNoAlerts=true")
    // console.log('//', data)
    // const items = this.getListItemsByVillage(data)

    // console.log('items', items)

    // const res = items.map(item => this.getParsedDataByMonth(item))

    // console.log('res', res)


    // const parsedData = this.getParsedDataByMonth([items[0]])

    // console.log('++', )

    this.setState({ 
      data: data.villages,
      dateRange: {
        startDate: data.from,
        endDate: data.to
      }
    })
  }

  getGroupByVillages = (data) => {
    return data.map(item => {
      return {
        id: item.village,
        title: item.village
      }
    })
  }

  getItems = (data) => {
    console.log('AA', data)
    const a = []
    data.forEach((village, y) => {
      // console.log('1', village)
      return village.alerts.forEach((alert, i) => {
        a.push({
          id: i + '-' + y,
          group: village.village,
          title: village.village + "-" + i,
          start_time: moment(alert.start_time),
          end_time: moment(alert.end_time)
        })
      })
    })

    return a
    // return data.forEach(village => {
    //   return village.alerts.map((alert, i) => {
    //     return {
    //       id: village.village + i,
    //       group: village.village,
    //       title: village.village + ' ' + i,
    //       start_time: moment(alert.startDate),
    //       end_time: moment(alert.endDate)
    //     }
    //   })
    // })
  }
  
  render() {

    // const groups = [{ id: 1, title: 'Viollage 1' }, { id: 2, title: 'village 2' }]
    const groups = this.getGroupByVillages(this.state.data)

    // const items = [
    //   {
    //     id: 1,
    //     group: 1,
    //     title: 'item 1',
    //     start_time: moment(),
    //     end_time: moment().add(1, 'hour')
    //   },
    //   {
    //     id: 2,
    //     group: 2,
    //     title: 'item 2',
    //     start_time: moment().add(-0.5, 'hour'),
    //     end_time: moment().add(0.5, 'hour')
    //   },
    //   {
    //     id: 3,
    //     group: 1,
    //     title: 'item 3',
    //     start_time: moment().add(2, 'hour'),
    //     end_time: moment().add(3, 'hour')
    //   }
    // ]
    const items = this.getItems(this.state.data)

    console.log('END', items)




   

    return (
      <div className="alert-history-view">
        <h1>Alert history:</h1>
        {/* <AlertHistoryContainer 
          dateRange={this.state.dateRange}
          label="Villages"
          data={this.state.data}
        /> */}
        <Timeline
          groups={groups}
          items={items}
          defaultTimeStart={moment().add(-12, 'hour')}
          defaultTimeEnd={moment().add(12, 'hour')}
          />
          {/* defaultTimeStart={moment(this.state.dateRange.startDate)} */}
          {/* defaultTimeEnd={moment(this.state.dateRange.endDate)} */}

      </div>
    )
  }
}

export default AlertHistoryView;