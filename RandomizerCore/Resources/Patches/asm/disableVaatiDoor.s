.thumb
push	{lr}

dhcBossRoomCheck:
ldr	r0, =#0x3000BF4
ldrb	r1, [r0, #0]
cmp	r1, #0x88
bne	end
ldrb	r1, [r0, #1]
cmp	r1, #0x14
bne	end

loadBlocker:
@block
mov	r0, #0x84
mov	r1, #1
mov	r2, #7
ldr	r3, =#0x807B5BC
mov	lr, r3
mov	r3, #1
.short	0xF800
@display sprite
ldr	r0, blocker
ldr	r3, =#0x804AAF8
mov	lr, r3
.short	0xF800

end:
mov	r0, #1
ldr	r3, =#0x804ae44
mov	lr, r3
.short	0xF800
ldr	r3, =#0x804ad40	@return to vanilla
mov	lr, r3
.short	0xF800

.align
.ltorg
blocker:
