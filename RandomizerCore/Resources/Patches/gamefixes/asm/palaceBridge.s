.thumb
push	{lr}
@check darknut flag
ldr	r0,=#0x2002DAB
ldrb	r0,[r0]
mov	r1,#0x40
and	r0,r1
cmp	r0,#0
bne	end

@load a skull that blocks the screen transition
ldr	r0,poin
ldr	r3,=#0x804AAF8 @LoadRoomEntityList
mov	lr,r3
.short	0xF800

end:
mov	r0,#0
ldr	r3,=#0x805A9FC @PowBackgroundManager_Main
mov	lr,r3
.short	0xF800
pop	{pc}
.align
.ltorg
poin:
