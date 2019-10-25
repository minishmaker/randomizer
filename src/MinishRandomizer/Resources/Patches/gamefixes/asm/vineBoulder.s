.thumb
push	{lr}
@check vine
ldr	r0,=#0x2002CC5
ldrb	r0,[r0]
mov	r1,#0x10
and	r0,r1
cmp	r0,#0
bne	end

@load the boulder
ldr	r0,poin
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

end:
pop	{pc}
.align
.ltorg
poin:
