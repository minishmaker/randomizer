.thumb
ldr r5, =#0x203FE00+(10*2)
ldrh	r5, [r5]
lsl	r5, #32 - 2
lsr	r5, #32 - 2
ldr	r0, =#0x8073EC6
mov	lr, r0
.short	0xF800