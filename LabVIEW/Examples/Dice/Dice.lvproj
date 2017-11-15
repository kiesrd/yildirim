﻿<?xml version='1.0' encoding='UTF-8'?>
<Project Type="Project" LVVersion="15008000">
	<Item Name="My Computer" Type="My Computer">
		<Property Name="server.app.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="server.control.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="server.tcp.enabled" Type="Bool">false</Property>
		<Property Name="server.tcp.port" Type="Int">0</Property>
		<Property Name="server.tcp.serviceName" Type="Str">My Computer/VI Server</Property>
		<Property Name="server.tcp.serviceName.default" Type="Str">My Computer/VI Server</Property>
		<Property Name="server.vi.callsEnabled" Type="Bool">true</Property>
		<Property Name="server.vi.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="specify.custom.address" Type="Bool">false</Property>
		<Item Name="calcCrcYldrm.vi" Type="VI" URL="../calcCrcYldrm.vi"/>
		<Item Name="Dice.vi" Type="VI" URL="../Dice.vi"/>
		<Item Name="initYldrm.vi" Type="VI" URL="../initYldrm.vi"/>
		<Item Name="pinToAscii.vi" Type="VI" URL="../pinToAscii.vi"/>
		<Item Name="readAnPinVal.vi" Type="VI" URL="../readAnPinVal.vi"/>
		<Item Name="readBgAnPinVal.vi" Type="VI" URL="../readBgAnPinVal.vi"/>
		<Item Name="readPinValYldrm.vi" Type="VI" URL="../readPinValYldrm.vi"/>
		<Item Name="setPinDirYldrm.vi" Type="VI" URL="../setPinDirYldrm.vi"/>
		<Item Name="setPinValYldrm.vi" Type="VI" URL="../setPinValYldrm.vi"/>
		<Item Name="Dependencies" Type="Dependencies">
			<Item Name="vi.lib" Type="Folder">
				<Item Name="VISA Configure Serial Port" Type="VI" URL="/&lt;vilib&gt;/Instr/_visa.llb/VISA Configure Serial Port"/>
				<Item Name="VISA Configure Serial Port (Instr).vi" Type="VI" URL="/&lt;vilib&gt;/Instr/_visa.llb/VISA Configure Serial Port (Instr).vi"/>
				<Item Name="VISA Configure Serial Port (Serial Instr).vi" Type="VI" URL="/&lt;vilib&gt;/Instr/_visa.llb/VISA Configure Serial Port (Serial Instr).vi"/>
				<Item Name="VISA Flush IO Buffer Mask.ctl" Type="VI" URL="/&lt;vilib&gt;/Instr/_visa.llb/VISA Flush IO Buffer Mask.ctl"/>
			</Item>
		</Item>
		<Item Name="Build Specifications" Type="Build"/>
	</Item>
</Project>
